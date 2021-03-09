import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr'

export default {
  install(Vue) {

    const connection = new HubConnectionBuilder()
      .withUrl(process.env.VUE_APP_MAKELINE_BASE)
      .configureLogging(LogLevel.Information)
      .build()

    let startedPromise = null
    function start() {
      startedPromise = connection.start().catch(err => {
        console.error('Failed to connect with hub', err)
        return new Promise((resolve, reject) =>
          setTimeout(() => start().then(resolve).catch(reject), 5000))
      })
      return startedPromise
    }
    connection.onclose(() => start())
    start();

    // use new Vue instance as an event bus
    const orderHub = new Vue()

    // every component will use this.$orderHub to access the event bus
    Vue.prototype.$orderHub = orderHub

    // Forward server side SignalR events through $orderHub, where components will listen to them

    connection.on("newOrder", message => {
      orderHub.$emit('new-order-received', message)
    }),
      connection.on("completedOrder", message => {
        orderHub.$emit('order-completed', message)
      })
  }
}