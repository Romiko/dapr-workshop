FROM node:lts-alpine

WORKDIR /app
COPY package*.json ./
RUN npm install

COPY . .

EXPOSE 80

CMD [ "npm", "start" ]