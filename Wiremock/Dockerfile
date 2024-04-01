FROM wiremock/wiremock:latest

COPY . /home/wiremock

ENTRYPOINT ["/docker-entrypoint.sh", "--global-response-templating", "--disable-gzip", "--verbose"]
