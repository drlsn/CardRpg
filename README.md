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
- Run server locally
  - By Docker - Install docker
  - Execute commands from command window
  &nbsp; &nbsp; 
  ```
  docker pull mongo:latest
  docker volume create mongodata
  docker run -v mongodata:/data/db -p 27018:27017 -d --name trinica-db mongo:latest 
  
  docker pull netspie/trinica:latest  
  docker run ^
  -e "TrinicaDatabaseConn=mongodb://localhost:27018" ^
  -e "ASPNETCORE_ENVIRONMENT=Development" ^
  -p 5166:8080 ^
  -d netspie/trinica:latest
  ```
- (optional) To allow testing on a mobile device, you must host server on local network
  - Open port 5166, ex. on Windows https://ec.europa.eu/digital-building-blocks/sites/display/CEKB/How+to+open+a+port+on+the+firewall
  - Open actual server project - \trinica-net-server\src\Trinica.sln
  - In /trinica-net-server/src/Trinica.Api/Properties/launchSettings.json add your http://<your-local-ip>:5166 address to proper section
- Run project in Unity (Play), Sign In/Sign Up or build and run for android
