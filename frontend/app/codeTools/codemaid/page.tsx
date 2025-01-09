import React, { useState } from "react";
import { Metadata } from "next";
import codeMaidModel from "@/Service/CodeMaid";
import Link from "next/link";

export const metadata: Metadata = {
  title: {
    absolute: "CodeMaid",
  },
};
export default async function Page() {
  const r = await codeMaidModel.GetApiProject();
  return (
    <div className="flex flex-col items-center justify-center">
      {r.map((x) => (
        <div className="m-3 w-8/12 p-10 ring-1" key={x.id}>
          <Link href={`codemaid/project/${x.id}`} className="" passHref>
            {x.name}
          </Link>
          <div>项目路径：{x.path}</div>
          <div>分支:{x.gitBranch}</div>
        </div>
      ))}
    </div>
  );
}
