# Unit tests

## App Tests

App tests run through Xamarin. Provided you can launch an iOS emulator and an Android emulator, you can run the app tests.

If you struggle to launch an emulator, you may be able to connect a device and test directly. (Known to be working with Android.)

## Portal and API Tests

Web and web service tests run with Playwright.

### Installing Playwright

You'll need `npm`, which you can install with `brew install npm` (you may need to prefix with `arch -arm64`).

Use npm to access the playwright repository, and install a browser:

```
npm -i -D @playwright/test
npx playwright install
```

### Launching through Visual Studio

On OS X, your instance of Visual Studio doesn't automatically get access to your shell's environment variables.
If there's an issue and the browser can't launch, try launching it from the terminal with:

```
open -n /Applications/Visual\ Studio.app
```

### Dockerised tests

* `SmokeTests.dockerfile` - defines and builds the smoke testing environment.
* `run-smoketests.sh` - launches and runs the smoke tests in a docker container.
