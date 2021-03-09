FROM maven:3.6.3-jdk-11 AS build
COPY loyalty-service/src /usr/src/app/src
COPY loyalty-service/pom.xml /usr/src/app
RUN mvn -f /usr/src/app/pom.xml clean package


FROM openjdk:11-jre-slim
VOLUME /tmp
ARG JAVA_OPTS
ENV JAVA_OPTS=$JAVA_OPTS
COPY --from=build /usr/src/app/target/loyalty-service-1.0-SNAPSHOT.jar /usr/app/loyalty-service-1.0-SNAPSHOT.jar
EXPOSE 80
ENTRYPOINT exec java $JAVA_OPTS -Djava.security.egd=file:/dev/./urandom -jar /usr/app/loyalty-service-1.0-SNAPSHOT.jar
