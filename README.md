# Desafio Radix
API em .NET Core 2.0, utilizando MySQL 5.7 e Docker.

Desenvolvido por **Daniel Amaral**.

### Pré-requisitos

Para rodar a aplicação, basta ter o Docker instalado na sua máquina.

## Instalação e execução

```
> git clone https://github.com/daniel-amaral/desafio-radix.git
> cd desafio-radix
> docker-compose up
```
Depois de alguns minutos, após a construção dos containers pelo Docker, a API já estará rodando. Se for a primeira vez que o sistema é executado, alguns dados serão mockados automaticamente.

Acesse a página gerada pelo Swagger para conferir todos os endpoints e seus contratos: http://*docker-machine-ip*:5000/swagger, geralmente [http://192.168.99.100:5000/swagger](http://192.168.99.100:5000/swagger).


![Swagger](https://github.com/daniel-amaral/desafio-radix/blob/master/web-api/swagger-screen.PNG)



Obrigado! **:)**
