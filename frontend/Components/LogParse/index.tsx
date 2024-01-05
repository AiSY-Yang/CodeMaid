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
}
const { TextArea } = Input
const pattern = /(@\w+)=\s*('[^']+')?(NULL)?/g
export default function Page() {
	const change = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
		var s = (e.target as HTMLTextAreaElement).value
		//判断日志类型
		if (s.indexOf('LogRecord') >= 0) {
			setLogType(LogType.OpenTelemetry)
		}
		var sqlData: SQLLog[] = Array<SQLLog>()
		var errorMessage: string[] = Array<string>()
		switch (logType) {
			case LogType.OpenTelemetry:
				/**下面的内容是否是sql */
				var isSQL: boolean = false
				var isError: boolean = false
				var startError: boolean = false
				var isErrorMessage: boolean
				var isParameter: boolean = true
				var tempSQL = ''
				//匹配到的sql参数
				var parameters: any = {}
				var parametersString: string = ''
				s.split('\n').map((line: string) => {
					//查找是否开始是sql
					var isSuccessSql = !isSQL && line.trim().indexOf('LogRecord.FormattedMessage:        Executed DbCommand') == 0
					var isErrorSql = !isSQL && line.trim().indexOf('LogRecord.FormattedMessage:        Failed executing DbCommand') == 0
					var lineIsSQL = isSuccessSql || isErrorSql
					//查找错误消息
					isError = isError || line.trim().indexOf('LogRecord.LogLevel:                Error') == 0
					startError = startError || (isError && (line.trim().indexOf('LogRecord.Exception') == 0 || (isError && line.trim().indexOf('LogRecord.FormattedMessage') == 0)))
					var endError = isError && (line.length == 0 || line.trim().indexOf('LogRecord.ScopeValues') == 0)
					//sql参数记录 当上一行不是sql但是这一行是的时候 说明开始是参数了
					if (!isSQL && lineIsSQL) {
						isSQL = true
						isParameter = true
						//当记录sql的时候不在当作错误消息
					}
					//如果是参数 就要开始记录参数字符串用于之后的匹配
					if (isParameter) {
						parametersString += line + '\r\n'
						//判断参数是不是结束了 如果结束了就要提取内容
						if (/CommandType=\'Text\', CommandTimeout=\'\d+\']$/.test(line)) {
							isParameter = false
							let match
							parameters = {}
							while ((match = pattern.exec(parametersString)) !== null) {
								const paramName = match[1]
								const paramValue = match[2]
								const NULL = match[3]
								if (NULL != undefined) parameters[paramName] = "null"
								if (paramValue != undefined) parameters[paramName] = paramValue
								// console.debug(`${paramName}=${parameters[paramName]}`)
							}
							// console.debug(parameters)
							parametersString = ''
						}
						return
					}
					if (isSQL && !isParameter) {
						if (line.trim().indexOf('LogRecord.') == 0) {
							lineIsSQL = false
							isSQL = false
							tempSQL = tempSQL.replaceAll(/@\w+/g, (match) => `${parameters[match]}`)
							sqlData.push({ Error: isErrorSql, sql: tempSQL.slice(0, -1) })
							// if (tempSQL.indexOf('undefin') > 0) console.debug(tempSQL)
						} else tempSQL += line + '\n'
						return
					}
					// console.debug(`${parametersString}`)

					tempSQL = ''
					if (isSQL)
						if (startError && !endError) {
							//错误消息记录
							errorMessage.push(line)
						}
				})
				break
			default:
				break
		}
		setSql(sqlData)
		setErrorMessageData(errorMessage)
		logDiv.current.scrollTop = logDiv.current.scrollHeight
		errorDiv.current.scrollTop = errorDiv.current.scrollHeight
	}
	var [sqlData, setSql] = useState(Array<SQLLog>)
	var [errorMessageData, setErrorMessageData] = useState(Array<string>)
	var [logType, setLogType] = useState(0)
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

	return (
		<>
			<div style={{ display: 'flex', height: '100vh' }}>
				<div style={{ flex: 1, marginRight: '50px' }}>
					<label>日志输入框</label>
					<TextArea id='logInput' autoSize={{ minRows: 10, maxRows: 50 }} placeholder='输入日志' onKeyUp={change} />
				</div>
				<div style={{ width: '50%', flex: 1, marginLeft: '50px', maxHeight: '100%', height: '100%', display: 'flex', flexDirection: 'column' }}>
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
