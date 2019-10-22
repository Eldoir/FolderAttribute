using System;
using UnityEngine;

namespace Folder
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FolderAttribute : PropertyAttribute
    {
       
    }

    public class FolderInfo
    {
        public string guid;
        public string path;

        public override string ToString()
        {
            return guid + " " + path;
        }
    }
}