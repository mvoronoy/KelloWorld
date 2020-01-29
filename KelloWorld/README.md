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
$> docker-compose up --build
```

Deploy to minikube:
Open PowerShell with Admin privelages
Execute (should be executed in each PS window each time, otherwise minikube will not be able to find image)
``` 
$> minikube docker-env | Invoke-Expression
```

Build new app image:
```
$> docker-compose build
```
Deploy mysql:
```
$> kubectl apply -f kubernetes\mysql.yml
```

Deploy app (should be deployed after db): 
```
$> kubectl apply -f .\kubernetes\application.yml
```

Access minikube dashboard to review results:
```
$> minikube dashboard
```
