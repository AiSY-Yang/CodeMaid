import Link from 'next/link'
import React from 'react'
import { Metadata } from 'next'
import { Divider } from 'antd'

export const metadata: Metadata = {
	title: {
		absolute: '代码工具',
	},
}
var items = [{ name: '日志解析器', desp: '解析C#控制台输出的日志信息,提取其中的SQL语句与报错', herf: './codeTools/logParse' }]
export default function Page() {
	return (
		<>
			<section className='text-gray-600 dark:text-gray-400 dark:bg-gray-900 body-font min-h-screen'>
				<div className='container px-5 py-24 mx-auto'>
					<h1 className='sm:text-3xl text-2xl font-medium title-font text-center text-gray-900 dark:text-white mb-20'>代码工具</h1>
					<div className='flex flex-wrap sm:-m-4 -mx-4 -mb-10 -mt-4 md:space-y-0 space-y-6'>
						{items.map((x) => (
							<div key={x.name} className='p-4 md:w-1/3 flex'>
								<div className='w-12 h-12 inline-flex items-center justify-center rounded-full bg-indigo-100 dark:bg-gray-800 text-indigo-500 dark:text-indigo-400 mb-4 flex-shrink-0'>
									<svg fill='none' stroke='currentColor' strokeLinecap='round' strokeLinejoin='round' strokeWidth='2' className='w-6 h-6' viewBox='0 0 24 24'>
										<path d='M22 12h-4l-3 9L9 3l-3 9H2'></path>
									</svg>
								</div>
								<div className='flex-grow pl-6'>
									<h2 className='text-gray-900 dark:text-white text-lg title-font font-medium mb-2'>{x.name}</h2>
									<p className='leading-relaxed text-base'>{x.desp}</p>
									<div className='mt-3 text-indigo-500 dark:text-indigo-400 inline-flex items-center'>
										<Link href={x.herf}>Link</Link>
										<svg fill='none' stroke='currentColor' strokeLinecap='round' strokeLinejoin='round' strokeWidth='2' className='w-4 h-4 ml-2' viewBox='0 0 24 24'>
											<path d='M5 12h14M12 5l7 7-7 7'></path>
										</svg>
									</div>
								</div>
							</div>
						))}
					</div>
				</div>
			</section>
		</>
	)
}
