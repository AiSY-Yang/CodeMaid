import React, { useState } from 'react'
import LogParse from '@/Components/LogParse'
import { Metadata } from 'next'

export const metadata: Metadata = {
	title: {
		absolute: '日志解析器',
	},
}
export default function Page() {
	const [project, setProject] = useState({})
	fetch()
	return <LogParse></LogParse>
}
