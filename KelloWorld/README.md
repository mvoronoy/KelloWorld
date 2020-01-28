To build single image: from directory "KelloWorld" (where Docker file is located) run the following
```
$> docker build .
```

See latest built image (by id output from previous command): 
```
$> docker images
```
Tag latest image:
```
$> docker tag 125f0140ff1a kelloWorld:0.0.1
```

Build using docker compose (application only): from root folder, where docker-compose.yml file is located
```
$> docker-compose build
```

Run application (with db) via docker-compose:
```
$> docker-compose up --buid
```