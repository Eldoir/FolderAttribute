using System;
using UnityEngine;

namespace Folder
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FolderAttribute : PropertyAttribute
    {

    }
}