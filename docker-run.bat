@echo off
echo Construindo e iniciando os contêineres Docker...
docker-compose up --build -d
echo.
echo Você pode acessar:
echo API: http://localhost:5000
echo Web: http://localhost:8080
echo.
echo Para ver os logs, execute: docker-compose logs -f
echo Para parar os contêineres, execute: docker-compose down
echo. 