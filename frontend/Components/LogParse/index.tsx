'use client'
import React, { useEffect, useRef, useState } from 'react'
import { Input } from 'antd'
import SyntaxHighlighter from 'react-syntax-highlighter'
import { darcula } from 'react-syntax-highlighter/dist/esm/styles/hljs'
import { debug } from 'console'
import { cwd } from 'process'

interface SQLLog {
	/**是否执行错误*/
	Error: boolean
	/**sql语句 */
	sql: string
}

enum LogType {
	'',
	OpenTelemetry,
	Default,
}
enum State {
	None,
	Parameter,
	SQL,
}

const { TextArea } = Input
const pattern = /(@\w+)=\s*('[^']+')?(NULL)?/g
export default function Page() {
	var [sqlData, setSql] = useState(Array<SQLLog>)
	var [errorMessageData, setErrorMessageData] = useState(Array<string>)
	var [logType, setLogType] = useState(0)
	var [endPoint, setEndpoint] = useState(Array<string>)
	const logDiv: any = useRef(null)
	const errorDiv: any = useRef(null)
	useEffect(() => {
		if (logDiv.current) {
			logDiv.current.scrollTop = logDiv.current.scrollHeight
		}
	}, [sqlData])
	useEffect(() => {
		if (errorDiv.current) {
			errorDiv.current.scrollTop = errorDiv.current.scrollHeight
		}
	}, [errorMessageData])
	const change = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
		var s = (e.target as HTMLTextAreaElement).value
		//判断日志类型
		var lines = s.split('\n')
		lines.forEach((element) => {
			if (element.indexOf('LogRecord.Timestamp') >= 0) {
				setLogType(LogType.OpenTelemetry)
				return
			}
			if (element.indexOf('warn: ') >= 0 || element.indexOf('info: ') >= 0) {
				setLogType(LogType.Default)
				return
			}
		})
		var sqlData: SQLLog[] = Array<SQLLog>()
		var errorMessage: string[] = Array<string>()
		/**结束标记 */
		var endPointSign: string
		/** 参数标记 出现就证明是个成功的sql */
		var parameterSign: string
		/** 错误参数标记 出现就证明是个失败的sql */
		var errorParameterSign: string
		/** sql结束判断 从这一行开始就不再是sql了 */
		var currentIsNotSql: (s: string) => boolean
		switch (logType) {
			case LogType.OpenTelemetry:
				endPointSign = 'LogRecord.FormattedMessage:        Now listening on: '
				parameterSign = 'LogRecord.FormattedMessage:        Executed DbCommand'
				errorParameterSign = 'LogRecord.FormattedMessage:        Failed executing DbCommand'
				currentIsNotSql = (line) => line.startsWith('LogRecord.')
				break
			case LogType.Default:
				endPointSign = 'Now listening on: '
				parameterSign = 'Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand'
				errorParameterSign = 'Failed executing DbCommand'
				currentIsNotSql = (line) => line.startsWith('info: ') || line.startsWith('warn: ') || line.startsWith('fail: ')
				break
			default:
				break
		}
		var state: State = State.None
		var isErrorSql = false
		var isParameter: boolean = true
		var tempSQL = ''
		//匹配到的sql参数
		var parameters: any = {}
		var parametersString: string = ''
		endPoint = []
		lines.map((line: string) => {
			line = line.trim()
			//判断是否是endpoint
			if (line.startsWith(endPointSign)) {
				endPoint.push(line.substring(endPointSign.length))
				return
			}
			if (line.startsWith('已加载“')) return
			function ParseParameter() {
				parametersString += `${line}\r\n`
				//判断参数是不是结束了 如果结束了就要提取内容
				if (/CommandType=\'Text\', CommandTimeout=\'\d+\']$/.test(line)) {
					parametersString += line + '\r\n'
					isParameter = false
					let match
					parameters = {}
					while ((match = pattern.exec(parametersString)) !== null) {
						const paramName = match[1]
						const paramValue = match[2]
						const NULL = match[3]
						if (NULL != undefined) parameters[paramName] = 'null'
						if (paramValue != undefined) parameters[paramName] = paramValue
						// console.debug(`${paramName}=${parameters[paramName]}`)
					}
					// console.debug(parameters)
					parametersString = ''
					state = State.SQL
				}
			}
			switch (state) {
				case State.None:
					if (line.startsWith(parameterSign) || line.startsWith(errorParameterSign)) {
						state = State.Parameter
						ParseParameter()
					}
					break
				case State.Parameter:
					ParseParameter()
					break
				case State.SQL:
					if (currentIsNotSql(line)) {
						state = State.None
						tempSQL = tempSQL.replaceAll(/@\w+/g, (match) => `${parameters[match]}`)
						sqlData.push({ Error: isErrorSql, sql: tempSQL.slice(0, -1) })
						tempSQL = ''
						break
					} else tempSQL += `${line}\r\n`
					break
			}
		})
		setSql(sqlData)
		setErrorMessageData(errorMessage)
		setEndpoint(endPoint)
		logDiv.current.scrollTop = logDiv.current.scrollHeight
		errorDiv.current.scrollTop = errorDiv.current.scrollHeight
	}

	return (
		<>
			<div style={{ display: 'flex', height: '100vh' }}>
				<div style={{ flex: 1, marginRight: '50px' }}>
					<label>日志输入框</label>
					<TextArea id='logInput' autoSize={{ minRows: 10, maxRows: 50 }} placeholder='输入日志' onKeyUp={change} />
				</div>
				<div style={{ width: '50%', flex: 1, marginLeft: '50px', maxHeight: '100%', height: '100%', display: 'flex', flexDirection: 'column' }}>
					<div className='flex'>
						EndPoint:
						<div className='inline'>
							{endPoint.map((x) => (
								<>
									<a key={x} href={x}>
										{x}
									</a>
									<br></br>
								</>
							))}
						</div>
					</div>
					<div style={{}}>日志形式:{LogType[logType]}</div>
					<div style={{}}>SQL语句:</div>
					<div ref={logDiv} style={{ overflowY: 'scroll' }}>
						{sqlData.map((sql, i) => (
							<>
								<div
									key={i}
									onClick={() => {
										navigator.clipboard.writeText(sql.sql)
									}}>
									<SyntaxHighlighter language='sql' style={darcula} customStyle={{ overflow: 'hidden', maxHeight: '100%' }} showLineNumbers>
										{sql.sql}
									</SyntaxHighlighter>
									<br></br>
								</div>
							</>
						))}
					</div>
					<div>错误消息</div>
					<div ref={errorDiv} style={{ height: '30%', overflowY: 'scroll', backgroundColor: 'gray', fontSize: 14 }}>
						{errorMessageData.map((x) => (
							<>
								<div key={x}>{x}</div>
							</>
						))}
					</div>
				</div>
			</div>
		</>
	)
}
