const { env } = require("process");

const target = env.ASPNETCORE_HTTPS_PORT
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(";")[0]
  : "http://localhost:48474";

const PROXY_CONFIG = [
  {
    context: ["/api/"],
    proxyTimeout: 10000,
    target: target,
    secure: false,
    headers: {
      host: "localhost",
      Connection: "Keep-Alive",
    },
  },
];

module.exports = PROXY_CONFIG;
