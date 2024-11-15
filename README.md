# FileCopyService

This document provides instructions for running, building, and installing the `FileCopyService` as a Windows Service or running it as a console application for debugging purposes.

---

## Running as a Console Application

To debug or run the service as a console application:

1. Open the project in **Visual Studio**.
2. Go to **Project Properties**:
   - Right-click the project in **Solution Explorer**.
   - Select **Properties**.
3. Navigate to the **Application** tab.
4. Set the **Output type** to `Console Application`.
5. Build and run the project (`F5`) to execute it in a console window.

---

## Installing as a Windows Service

To deploy the service as a Windows Service:

1. Open the project in **Visual Studio**.
2. Go to **Project Properties**:
   - Right-click the project in **Solution Explorer**.
   - Select **Properties**.
3. Navigate to the **Application** tab.
4. Set the **Output type** to `Windows Application`.
5. Build the project (preferably in **Release** mode).

---

## Steps to Build and Install the Service

Follow these steps to build a release version and install the service:

1. **Set Build Configuration**:
   - In Visual Studio, set the build configuration to **Release** (use the dropdown in the toolbar).

2. **Build the Project**:
   - Press `Ctrl + Shift + B` to build the project.
   - Locate the built executable file in `bin\Release`.

3. **Open Command Prompt as Administrator**:
   - Press `Win + S`, type `cmd`, and select **Run as Administrator**.

4. **Install the Service**:
   - Use the `sc` command to create and install the service. For example:
     ```cmd
     sc create FileCopyService binPath= "C:\Users\juanb\OneDrive\Documentos\Wedbush Repos\FileCopyService\bin\Release\FileCopyService.exe" start= auto
     ```

5. **Start the Service**:
   - Run the following command to start the service:
     ```cmd
     sc start FileCopyService
     ```

6. **Verify Installation**:
   - Open the **Services Console** (`services.msc`) and verify that the service (`FileCopyService`) is running.

---

## Example Installation Command

Here is an example command to install the service, assuming the executable is located in the `Debug` folder:

```cmd
sc create FileCopyService binPath= "C:\Users\juanb\OneDrive\Documentos\Wedbush Repos\FileCopyService\bin\Debug\FileCopyService.exe" start= auto
