How to send message to telegram after deloy
	https://github.com/marketplace/actions/telegram-notify
	https://dev.to/felipemsfg/telegram-notification-with-github-actions-69m
	Find out your TELEGRAM_ID. Just search for @userinfobot and start a conversation with /star message. The result message will be like that:
	@your_telegram_username
	Id: 12345678 ( fill id in key name is "to" )
	First: <Your Firstname>
	Last: <Your Lastname>
	Lang: en-US






version: '3.0'

services:
    website-database:
        image: mysql:8.0.33
        container_name: website-database
        restart: always
        networks:
            - website-network
        volumes:
            - data:/var/lib/mysql
            - ./_MySQL_Init_Script:/docker-entrypoint-initdb.d
        environment:
            - MYSQL_DATABASE=${MYSQL_DATABASE}
            - MYSQL_ROOT_USER=${MYSQL_ROOT_USER}
            - MYSQL_ROOT_PASSWORD=${MYSQL_ROOT_PASSWORD}
            - MYSQL_USER=${MYSQL_USER}
            - MYSQL_PASSWORD=${MYSQL_PASSWORD}
        ports:
            - 3306:3306
volumes:
    data:
    
networks:
    website-network:
        driver: bridge
        name: website-network
