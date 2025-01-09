import { $Fetch, ofetch } from 'ofetch'
import option from './CodeMaidOptions'
import * as Model from './CodeMaidModel'

class CodeMaid {
	/** The client for making HTTP requests */
	client: $Fetch
	constructor() {
		this.client = ofetch.create({ baseURL: option.baseURL })
	}
	Get<Main>b__6_24({  } : {  }): Promise<string | undefined> {
		return this.client('/version', {
			method: 'Get',
		})
	}
	PostCommandCommand({ body } : { body: any }): Promise<Model.TaskServiceControllerCommandControllerCommandOutPut> {
		return this.client('/api/Command/Command', {
			method: 'Post',
			body: body,
		})
	}
	GetDirectoryExist({ params } : { params: {path: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/api/Directory/Exist', {
			method: 'Get',
			params: params,
		})
	}
	PostDirectoryCreate({ params } : { params: {path: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/api/Directory/Create', {
			method: 'Post',
			params: params,
		})
	}
	PostDirectoryDelete({ params } : { params: {path: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/api/Directory/Delete', {
			method: 'Post',
			params: params,
		})
	}
	GetDirectorySize({ params } : { params: {path: string | undefined} }): Promise<number | undefined> {
		return this.client('/api/Directory/Size', {
			method: 'Get',
			params: params,
		})
	}
	GetDirectoryContent({ params } : { params: {path: string | undefined; needFile: boolean | undefined; recursive: boolean | undefined} }): Promise<Model.ApiControllersCommonsSystemFolder> {
		return this.client('/api/Directory/Content', {
			method: 'Get',
			params: params,
		})
	}
	GetDirectoryFile({ params } : { params: {path: string | undefined} }): Promise<string[]> {
		return this.client('/api/Directory/File', {
			method: 'Get',
			params: params,
		})
	}
	PutDirectoryMove({ params } : { params: {path: string | undefined; to: string | undefined} }): Promise<string | undefined> {
		return this.client('/api/Directory/Move', {
			method: 'Put',
			params: params,
		})
	}
	PutDirectoryCopy({ params } : { params: {path: string | undefined; to: string | undefined} }): Promise<string | undefined> {
		return this.client('/api/Directory/Copy', {
			method: 'Put',
			params: params,
		})
	}
	GetDirectoryZip({ params } : { params: {path: string | undefined} }): Promise<Blob> {
		return this.client('/api/Directory/Zip', {
			method: 'Get',
			params: params,
		})
	}
	GetFileExist({ params } : { params: {path: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/File/Exist', {
			method: 'Get',
			params: params,
		})
	}
	GetFileSize({ params } : { params: {path: string | undefined} }): Promise<number | undefined> {
		return this.client('/File/Size', {
			method: 'Get',
			params: params,
		})
	}
	GetFileDownload({ params } : { params: {path: string | undefined} }): Promise<Blob> {
		return this.client('/File/Download', {
			method: 'Get',
			params: params,
		})
	}
	PostFileWriteText({ body } : { body: any }): Promise<string | undefined> {
		return this.client('/File/WriteText', {
			method: 'Post',
			body: body,
		})
	}
	PostFileWrite({ params, body } : { params: {directory: string | undefined}; body: any }): Promise<string | undefined> {
		return this.client('/File/Write', {
			method: 'Post',
			params: params,
			body: body,
		})
	}
	PostFileSyncWrite({ params } : { params: {filePath: string | undefined} }): Promise<string | undefined> {
		return this.client('/File/SyncWrite', {
			method: 'Post',
			params: params,
		})
	}
	GetFileTextContent({ params } : { params: {filePath: string | undefined} }): Promise<string | undefined> {
		return this.client('/File/TextContent', {
			method: 'Get',
			params: params,
		})
	}
	GetFileTextContents({ params } : { params: {filePaths: string[]} }): Promise<object | undefined> {
		return this.client('/File/TextContents', {
			method: 'Get',
			params: params,
		})
	}
	DeleteFileDelete({ params } : { params: {filePath: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/File/Delete', {
			method: 'Delete',
			params: params,
		})
	}
	PutFileMove({ params } : { params: {sourceFileName: string | undefined; destinationFileName: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/File/Move', {
			method: 'Put',
			params: params,
		})
	}
	PutFileMoveAndOverWrite({ params } : { params: {sourceFileName: string | undefined; destinationFileName: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/File/MoveAndOverWrite', {
			method: 'Put',
			params: params,
		})
	}
	PutFileCopy({ params } : { params: {sourceFileName: string | undefined; destinationFileName: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/File/Copy', {
			method: 'Put',
			params: params,
		})
	}
	PutFileCopyAndOverWrite({ params } : { params: {sourceFileName: string | undefined; destinationFileName: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/File/CopyAndOverWrite', {
			method: 'Put',
			params: params,
		})
	}
	PutFileLink({ params } : { params: {sourceFileName: string | undefined; destinationFileName: string | undefined} }): Promise<boolean | undefined> {
		return this.client('/File/Link', {
			method: 'Put',
			params: params,
		})
	}
	PostFileDecompressionZip({ params } : { params: {zipFileName: string | undefined; destinationDirectory: string | undefined} }): Promise<string[]> {
		return this.client('/File/DecompressionZip', {
			method: 'Post',
			params: params,
		})
	}
	PostFunctionSetRemoteSSHCertAuth({  } : {  }): Promise<boolean | undefined> {
		return this.client('/api/Function/SetRemoteSSHCertAuth', {
			method: 'Post',
		})
	}
	GetProjectGetList({  } : {  }): Promise<ModelsCodeMaidProject[]> {
		return this.client('/api/Project', {
			method: 'Get',
		})
	}
	GetProjectGetDetail({  } : {  }): Promise<Model.ModelsCodeMaidProject> {
		return this.client('/api/Project/{id}', {
			method: 'Get',
		})
	}
	PutProjectFlushAllFile({ params } : { params: {id: number | undefined} }): Promise<boolean | undefined> {
		return this.client('/api/Project/FlushAllFile', {
			method: 'Put',
			params: params,
		})
	}
	PostProjectGitHooks({ params } : { params: {id: number | undefined} }): Promise<boolean | undefined> {
		return this.client('/api/Project/GitHooks', {
			method: 'Post',
			params: params,
		})
	}
	GetSystemGetEnumDictionaries({  } : {  }): Promise<object | undefined> {
		return this.client('/api/System/GetEnumDictionaries', {
			method: 'Get',
		})
	}
	GetSystemGetControllers({  } : {  }): Promise<ApiControllersCommonsControllerInfo[]> {
		return this.client('/api/System/GetControllers', {
			method: 'Get',
		})
	}
	GetSystemEcho({ params } : { params: {statusCode: number | null | undefined; delay: number | null | undefined} }): Promise<Blob> {
		return this.client('/api/System/Echo', {
			method: 'Get',
			params: params,
		})
	}
	PostSystemEcho({ params } : { params: {statusCode: number | null | undefined; delay: number | null | undefined} }): Promise<Blob> {
		return this.client('/api/System/Echo', {
			method: 'Post',
			params: params,
		})
	}
	PutSystemEcho({ params } : { params: {statusCode: number | null | undefined; delay: number | null | undefined} }): Promise<Blob> {
		return this.client('/api/System/Echo', {
			method: 'Put',
			params: params,
		})
	}
	DeleteSystemEcho({ params } : { params: {statusCode: number | null | undefined; delay: number | null | undefined} }): Promise<Blob> {
		return this.client('/api/System/Echo', {
			method: 'Delete',
			params: params,
		})
	}
	HeadSystemEcho({ params } : { params: {statusCode: number | null | undefined; delay: number | null | undefined} }): Promise<Blob> {
		return this.client('/api/System/Echo', {
			method: 'Head',
			params: params,
		})
	}
	OptionsSystemEcho({ params } : { params: {statusCode: number | null | undefined; delay: number | null | undefined} }): Promise<Blob> {
		return this.client('/api/System/Echo', {
			method: 'Options',
			params: params,
		})
	}
	PatchSystemEcho({ params } : { params: {statusCode: number | null | undefined; delay: number | null | undefined} }): Promise<Blob> {
		return this.client('/api/System/Echo', {
			method: 'Patch',
			params: params,
		})
	}
	GetSystemForward({ params } : { params: {forward: string | undefined} }): Promise<Blob> {
		return this.client('/api/System/Forward', {
			method: 'Get',
			params: params,
		})
	}
	PostSystemForward({ params } : { params: {forward: string | undefined} }): Promise<Blob> {
		return this.client('/api/System/Forward', {
			method: 'Post',
			params: params,
		})
	}
	PutSystemForward({ params } : { params: {forward: string | undefined} }): Promise<Blob> {
		return this.client('/api/System/Forward', {
			method: 'Put',
			params: params,
		})
	}
	DeleteSystemForward({ params } : { params: {forward: string | undefined} }): Promise<Blob> {
		return this.client('/api/System/Forward', {
			method: 'Delete',
			params: params,
		})
	}
	HeadSystemForward({ params } : { params: {forward: string | undefined} }): Promise<Blob> {
		return this.client('/api/System/Forward', {
			method: 'Head',
			params: params,
		})
	}
	OptionsSystemForward({ params } : { params: {forward: string | undefined} }): Promise<Blob> {
		return this.client('/api/System/Forward', {
			method: 'Options',
			params: params,
		})
	}
	PatchSystemForward({ params } : { params: {forward: string | undefined} }): Promise<Blob> {
		return this.client('/api/System/Forward', {
			method: 'Patch',
			params: params,
		})
	}
	GetSystemKeepAlive({  } : {  }): Promise<Blob> {
		return this.client('/api/System/KeepAlive', {
			method: 'Get',
		})
	}
}
const CodeMaidClient = new CodeMaid()
export default CodeMaidClient
