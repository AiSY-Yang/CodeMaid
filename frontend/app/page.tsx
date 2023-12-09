'use client'
import Link from 'next/link'
import { cwd } from 'process'
import { useState } from 'react'
import { useRouter } from 'next/navigation'
interface Item {
	key: string
	name: string
	desp: string
	herf: string
	child?: Item[] | null | undefined
}
const data = [
	{ key: 'codeTool', name: '代码工具', desp: '一些写代码时用到的小玩意', herf: '.', child: [{ key: 'logparse', name: '日志解析器', desp: '解析C#控制台输出的日志信息,提取其中的SQL语句与报错', herf: './codeTools/logParse' }] },
	{
		key: 'clone',
		name: '赛博分身',
		desp: '感谢科技',
		herf: '.',
		child: [
			{ key: 'mouse', name: '嘴替', desp: '变声,TTS 邀请码 AgTBt0T', herf: 'https://dubbing.tech' },
			{ key: 'leg', name: '腿替', desp: '游遍全世界', herf: 'https://www.skylinewebcams.com/' },
		],
	},
]
function findLastArray(items: Array<Item> | null | undefined, a: Array<string>) {
	if (items == undefined || items == null) return null
	if (a.length == 0) return items
	var x = items.find((x) => x.key == a[0])
	if (x == undefined || x == null) return null
	console.log('x', x)
	if (a.length == 1) {
		return x.child
	}
	// 否则，递归遍历 current 的 child 属性
	a.shift()
	return findLastArray(x.child, a)
}

export default function Home() {
	const router = useRouter()
	const query = new URLSearchParams(window.location.search)
	console.log('query', query.getAll('key'))
	var items = findLastArray(data, query.getAll('key'))
	console.log('items', items)
	if (items == null) return <></>
	items.map((x) => {
		if ((x.herf = '.')) {
			query.set('key', x.key)
			x.herf = window.location + '?' + query.toString()
			console.log('x.herf', x.herf)
		}
	})
	console.log('items', items)
	return (
		<main>
			<section className='text-gray-600 dark:text-gray-400 dark:bg-gray-900 body-font min-h-screen'>
				<div className='container px-5 py-24 mx-auto'>
					<h1 className='sm:text-3xl text-2xl font-medium title-font text-center text-gray-900 dark:text-white mb-20'>CodeMaid</h1>
					<div className='flex flex-wrap sm:-m-4 -mx-4 -mb-10 -mt-4 md:space-y-0 space-y-6'>
						{items.map((x) => (
							<div key={x.name} className='p-4 md:w-1/3 flex hover:ring'>
								<div className='w-12 h-12 inline-flex items-center justify-center rounded-full bg-indigo-100 dark:bg-gray-800 text-indigo-500 dark:text-indigo-400 mb-4 flex-shrink-0'>
									<svg fill='none' stroke='currentColor' strokeLinecap='round' strokeLinejoin='round' strokeWidth='2' className='w-6 h-6' viewBox='0 0 24 24'>
										<path d='M22 12h-4l-3 9L9 3l-3 9H2'></path>
									</svg>
								</div>
								<div
									className='flex-grow pl-6'
									onClick={(d) => {
										router.push(x.herf, { scroll: true })
										// if (Array.isArray(x.child)) setitems(x.child)
									}}>
									<h2 className='text-gray-900 dark:text-white text-lg title-font font-medium mb-2'>{x.name}</h2>
									<p className='leading-relaxed text-base'>{x.desp}</p>
									<div className='mt-3 text-indigo-500 dark:text-indigo-400 inline-flex items-center'>
										{(x.child != undefined && x.child != null && x.child.length != 0) || (
											<>
												<Link href={x.herf} replace={x.child == undefined || x.child == null || x.child.length == 0}>
													Link
												</Link>
												{/* {x.herf} */}
												<svg fill='none' stroke='currentColor' strokeLinecap='round' strokeLinejoin='round' strokeWidth='2' className='w-4 h-4 ml-2' viewBox='0 0 24 24'>
													<path d='M5 12h14M12 5l7 7-7 7'></path>
												</svg>
											</>
										)}
									</div>
								</div>
							</div>
						))}
					</div>
				</div>
			</section>
		</main>
	)
}
