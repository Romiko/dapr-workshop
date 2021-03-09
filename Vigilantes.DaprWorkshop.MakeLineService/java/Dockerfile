FROM maven:3.6.3-jdk-11 AS build
COPY makeline-service/src /usr/src/app/src
COPY makeline-service/pom.xml /usr/src/app
RUN mvn -f /usr/src/app/pom.xml clean package


FROM openjdk:11-jre-slim
VOLUME /tmp
ARG JAVA_OPTS
ENV JAVA_OPTS=$JAVA_OPTS
COPY --from=build /usr/src/app/target/makeline-service-0.0.1-SNAPSHOT.jar /usr/app/makeline-service-0.0.1-SNAPSHOT.jar
EXPOSE 80
ENTRYPOINT exec java $JAVA_OPTS -Djava.security.egd=file:/dev/./urandom -jar /usr/app/makeline-service-0.0.1-SNAPSHOT.jar
