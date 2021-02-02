# Capto Glove SDK

## Release changelog

### Version 1.3.0. Oct the 26-th, 2019

- Fixed crash in some conditions on stream receiving
- Improved stability of connection for Windows
- Breaking changes in API
- Works with Firmware 158+

### Version 1.2.5. Feb the 19-th, 2019

- Fixes of fast call stop after start does not cause real stop
- Works with Firmware 158+

### Version 1.2.4. Feb the 2-d, 2019

- Supported SDK 181. Supported RepeatDelay for triggers. Supported MotionControllerConfig.
- Works with Firmware 158+

### Version 1.2.3. Jan the 26-th, 2019

- Works with Firmware 158+

### Version 1.2.2. Dec the 23-th, 2018

- Added new namespace GSdkNet.Base.BoardSetup, containing high level saving all data from board to xml document, understooden by CaptoGlove Suite.
- GSdkNet.Base.Board.Tools moved to GSdkNet.Base.Tools
- Works with Firmware 178+

### Version 1.2.1. Oct the 10-th, 2018

- Fixed warnings about corrupted system reserved bluetoothservices
- Works with Firmware 168+

### Version 1.2.0. Aug the 31-th, 2018

- Fixed missed GSdkCxx library error
- Fixed Unity freezing in editor mode
- Now public interface is async by default
- Works with Firmware 168+

### Version 1.1.8. Apr the 28-th, 2018

- Now SDK is universal, both for .Net and Unity
- Now SDK supports both x32 and x64 Win platforms
- Now scaning interface is asynchronous
- Added possibility to read user info, like glove wearable position
- Unity library is distributing via package
- Added additional SensorConfiguration presets
- Fixed acceleration stream wrong values
- Finger stream now is more smooth
- Works with Firmware 158+

### Version 1.1.7. Apr the 05-th, 2018

- Simplifing interface
- Improved stability

### Version 1.1.6. Mar the 01-t, 2018

- Improved stability

### Version 1.1.5. Feb the 19-th, 2018

- Added name reading
- Improved stability

### Version 1.1.4. Feb the 7-th, 2018

- Added user-friendly descriptions for raising exception in incorrect glove working operations.
- Fixed issue with incoming incorrect tared quaternion stream.

### Version 1.1.3. Feb the 5-th 2018

- Added possibility to configure Sensor descriptors.
- GSdkMarshaling (working name) was finally renamed to GSdkCortex.

## Installing crash log handler to catch library issues

In case of encountering crash inside SDK, please collect crash logs, using unhandled exception handler as it is written below. 

```
using System;
using System.Threading;
using System.Windows.Forms;
using NLog;

static class UnhandledExceptionHandler {
        public static void Start() {
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            Application.ThreadException += GlobalThreadExceptionHandler;
        }

        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        private static void LogException(Exception ex) {
            logger.Error("CRASH: " + ex.GetType() + ": " + ex.Message + "\n" + ex.StackTrace);
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e) {
            var ex = (Exception)e.ExceptionObject;
            LogException(ex);
        }

        private static void GlobalThreadExceptionHandler(object sender, ThreadExceptionEventArgs e) {
            var ex = e.Exception;
            LogException(ex);
        }
    }
```

Below is an example of it usage in desktop Windows application.

```
public partial class App : Application {
	protected override void OnStartup(StartupEventArgs e) {
		UnhandledExceptionHandler.Start();
	}
}
```

For unity application it can be initialized as follows:

```
public class MyScript : MonoBehaviour {
    void Start() {
    	UnhandledExceptionHandler.Start();
    }
}
```