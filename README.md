
# FileCopyService

This document provides comprehensive instructions for running, building, installing, stopping, uninstalling, and managing the `FileCopyService`.

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

## Steps to Build, Install, Stop, and Uninstall the Service

### 1. Build the Service

Follow these steps to build a release version of the service:

1. **Set Build Configuration**:
   - In Visual Studio, set the build configuration to **Release** (use the dropdown in the toolbar).

2. **Build the Project**:
   - Press `Ctrl + Shift + B` to build the project.
   - Locate the built executable file in `bin\Release`.

---

### 2. Install the Service

To install the service:

1. **Open Command Prompt as Administrator**:
   - Press `Win + S`, type `cmd`, and select **Run as Administrator**.

2. **Install the Service**:
   - Use the `sc` command to create and install the service. For example:
     ```cmd
     sc create FileCopyService binPath= "C:\Users\juanb\OneDrive\Documentos\Wedbush Repos\FileCopyService\bin\Release\FileCopyService.exe" start= auto
     ```

---

### 3. Start the Service

To start the service:

```cmd
sc start FileCopyService
```

---

### 4. Stop the Service

To stop the service:

```cmd
sc stop FileCopyService
```

---

### 5. Uninstall (Delete) the Service

To uninstall the service:

```cmd
sc delete FileCopyService
```

---

### 6. Verify Installation

To verify that the service is installed:

1. Open the **Services Console**:
   - Press `Win + R`, type `services.msc`, and press Enter.
2. Locate the service (`FileCopyService`) in the list.
3. Check its status (e.g., "Running").

---

## Example Installation Command

Here is an example command to install the service, assuming the executable is located in the `Debug` folder:

```cmd
sc create FileCopyService binPath= "C:\Users\juanb\OneDrive\Documentos\Wedbush Repos\FileCopyService\bin\Debug\FileCopyService.exe" start= auto
```

---

## Managing the Service

### To Start the Service:
```cmd
sc start FileCopyService
```

### To Stop the Service:
```cmd
sc stop FileCopyService
```

### To Uninstall the Service:
```cmd
sc delete FileCopyService
```

---

## Notes

- **Administrator Privileges**: Ensure you run the Command Prompt as Administrator for installation, starting, stopping, and uninstalling the service.
- **Output Types**:
  - Use the **Console Application** output type for debugging or running interactively.
  - Use the **Windows Application** output type when deploying as a Windows Service.
- **Error Handling**: Check the logs (if implemented) for errors if the service fails to start or stops unexpectedly.
