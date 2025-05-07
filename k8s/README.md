# Manifesto Kubernetes para Hackaton

Este diretório contém os manifestos Kubernetes necessários para implantar a aplicação Hackaton em um cluster Kubernetes com alta disponibilidade e escalabilidade.

## Estrutura dos Manifestos

- **namespace.yaml**: Define o namespace `hackaton` para isolar os recursos
- **configmap.yaml**: Configurações para a aplicação
- **secret.yaml**: Senhas e informações sensíveis
- **db-pvc.yaml**: PersistentVolumeClaim para armazenamento do banco de dados
- **db-deployment.yaml**: Deployment para o SQL Server
- **db-service.yaml**: Service para o SQL Server
- **api-deployment.yaml**: Deployment para a API
- **api-service.yaml**: Service para a API
- **api-hpa.yaml**: HorizontalPodAutoscaler para escalar a API
- **web-configmap.yaml**: ConfigMap com configuração do NGINX
- **web-deployment.yaml**: Deployment para a aplicação web
- **web-service.yaml**: Service para a aplicação web
- **web-hpa.yaml**: HorizontalPodAutoscaler para escalar a aplicação web
- **ingress.yaml**: Ingress para expor a aplicação
- **kustomization.yaml**: Arquivo Kustomize para facilitar a implantação

## Implantação

Para implantar a aplicação, siga os passos abaixo:

1. Certifique-se de ter o kubectl configurado para acessar seu cluster Kubernetes

2. Crie o namespace:
   ```
   kubectl apply -f namespace.yaml
   ```

3. Implante todos os recursos usando o Kustomize:
   ```
   kubectl apply -k .
   ```

4. Verifique se todos os recursos foram criados corretamente:
   ```
   kubectl get all -n hackaton
   ```

5. Adicione uma entrada no arquivo hosts para acessar a aplicação:
   ```
   echo "127.0.0.1 hackaton.local" | sudo tee -a /etc/hosts
   ```

6. Acesse a aplicação em http://hackaton.local

## Escalabilidade

A aplicação foi configurada para escalar automaticamente:

- A API começa com 3 réplicas e pode escalar até 10 réplicas
- A aplicação web começa com 3 réplicas e pode escalar até 10 réplicas
- O escalonamento é baseado na utilização de CPU (70%) e memória (80%)

## Recursos

Cada componente tem limites de recursos definidos:

- **API**: 200m-500m CPU, 256Mi-512Mi memória
- **Web**: 100m-300m CPU, 128Mi-256Mi memória
- **DB**: 500m-1 CPU, 2Gi-4Gi memória

## Monitoramento

Para monitorar o escalonamento:

```
kubectl get hpa -n hackaton
``` 