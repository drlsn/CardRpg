# Trinica - Unity Client

&nbsp;&nbsp;&nbsp;&nbsp; This is client side Unity project of the Trinica card game.

### Environment Setup

#### Development

- Download repositories
  - https://github.com/drlsn/trinica-unity-client
  - https://github.com/drlsn/trinica-net-server
- Open Unity client project scene - /trinica-unity-client/src/Assets/Scenes/LoadingScreen.scene
- Switch Platform to Android -
  - Go to File->Build Settings
  - Select Platform->Android and select Switch Platform
- Provide project keystore
  - Go to File->Build Settings->Player Settings->Publishing Settings
  - Select proper keystore (trinica.dev.keystore, secret) 
  - Provide password (secret)
- Open and run server project - /trinica-net-server/src/Trinica.sln
- (optional) To allow testing on a mobile device, you must host server on local network
  - Open port 5166, ex. on Windows https://ec.europa.eu/digital-building-blocks/sites/display/CEKB/How+to+open+a+port+on+the+firewall
  - In /trinica-net-server/src/Trinica.Api/Properties/launchSettings.json add your http://<your-local-ip>:5166 address to proper section
