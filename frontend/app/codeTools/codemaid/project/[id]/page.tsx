import React, { useState } from "react";
import { Metadata } from "next";
import codeMaidModel from "@/Service/CodeMaid";
import Link from "next/link";

export const metadata: Metadata = {
  title: {
    absolute: "CodeMaid",
  },
};
export default async function Page({ params  }: { params : {id:number} }) {
  const r = await codeMaidModel.getapr();
  return <div className="flex flex-col items-center justify-center">project</div>;F
}
