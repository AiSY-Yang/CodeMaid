using MaidContexts;

using Microsoft.AspNetCore.Mvc;

using Models.CodeMaid;

using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Api.Controllers
{
	public class ProjectController : ApiControllerBase
	{
		private readonly MaidContext maidContext;

		public ProjectController(MaidContext maidContext)
		{
			this.maidContext = maidContext;
		}
		[HttpGet("")]
		public List<Project> GetList()
		{
			return maidContext.Projects.ToList();
		}

		[HttpPut("[action]")]
		public bool FlushAllFile(long id)
		{
			return true;
		}
		[HttpPost("[action]")]
		public bool GitHooks(long id)
		{
			var function_py = """"
				import subprocess

				def execute_command(cmd: str):
				    """
				    execute a shell command and return the result
				    """
				    process = subprocess.run(cmd, stdout=subprocess.PIPE, shell=True)
				    if process.returncode != 0:
				        print('执行命令', cmd, '失败,返回值', process.returncode)
				        print(process.stderr)
				        exit(1)
				    return process
				def execute_command_return_output(cmd: str):
				    """
				    execute a shell command and return the result
				    """
				    process = subprocess.run(cmd, stdout=subprocess.PIPE, shell=True)
				    if process.returncode != 0:
				        print('执行命令', cmd, '失败,返回值', process.returncode)
				        print(process.stderr)
				        exit(1)
				    return process.stdout.decode("utf-8")
				
				"""";
			var pre_commit = """
				#!/usr/bin/env python
				import os
				import sys
				import function
				try:

				    # 检查是否有冲突 在处理冲突的时候跳过一切校验
				    if (not os.path.exists(".git/MERGE_HEAD")):
				        # 获取分支
				        process = function.execute_command(
				            'git rev-parse --symbolic --abbrev-ref HEAD')
				        branch = process.stdout.decode("utf-8").strip()
				        # 获取远程分支 当远程存在此分支的时候拉最新的代码 让同一分支保持一条线
				        p = function.execute_command('git branch -r')
				        if (branch in p.stdout.decode("utf-8")):
				            function.execute_command('git pull')
				        # 设定不带-的分支为主分支 禁止提交
				        if (('-' not in branch) and branch != 'az'):
				            print(".git/hooks: 不能commit到 {} 分支".format(branch))
				            exit(1)

				except:
				    print(repr(sys.exception()))
				    exit(1)
				""";
			var pre_push = """
				#!/usr/bin/env python
				import sys
				import function
				try:
				    branch = function.execute_command_return_output(
				        'git rev-parse --symbolic --abbrev-ref HEAD').strip()
				    if ('-' in branch):
				        kv = branch.split("-")
				        function.execute_command('git checkout '+kv[1])
				        function.execute_command('git pull ')
				        res = function.execute_command('git merge '+branch)
				        function.execute_command('git push ')
				        function.execute_command('git checkout '+branch)
				except:
				    print(repr(sys.exception()))
				    exit(1)
				""";
			return true;
		}

	}
}
