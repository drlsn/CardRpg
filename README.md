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
  docker volume create trinica-db-1
  docker volume create trinica-db-2
  docker network create trinica-db
  docker run --name trinica-db-1 -d -p 27018:27017 -v trinica-db-1:/data/db --net=trinica-db mongo --replSet rs
  docker run --name trinica-db-2 -d -v trinica-db-2:/data/db --net=trinica-db mongo --replSet rs
  docker exec -it trinica-db-1 mongosh
  rs.initiate({
  	_id: "rs",
  	members: [
  		{ _id: 0, host: "trinica-db-1" },
  		{ _id: 1, host: "trinica-db-2" }
  	]
  })
  rs.status()
  exit

  docker pull netspie/trinica:latest  
  docker run ^
  -e "TrinicaDatabaseConn=mongodb://localhost:27018" ^
  -e "ASPNETCORE_ENVIRONMENT=Development" ^
  -p 5166:8080 ^
  -d netspie/trinica:latest
  ```
- Set 
- (optional) To allow testing on a mobile device, so it can connect to the server you must open port on which the server container was run
  - Open port 5166, ex. on Windows https://ec.europa.eu/digital-building-blocks/sites/display/CEKB/How+to+open+a+port+on+the+firewall
- Run project in Unity (Play), Sign In/Sign Up or build and run for android
