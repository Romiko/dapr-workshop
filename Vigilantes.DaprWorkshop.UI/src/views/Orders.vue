<template>
  <el-container>
    <el-header width="100%" height="100%" style="margin:30px;">
      <p>Orders Completed (%)</p>
      <el-progress :text-inside="true" :stroke-width="30" :percentage="percentage" :status="status"></el-progress>
    </el-header>
    <el-header height="100px" style="margin-top:50px">
      <div class="right">
        <el-tooltip class="item" effect="dark" content="Fetch new orders" placement="right">
          <el-button @click="refreshData">Refresh</el-button>
        </el-tooltip>
      </div>
      <img style="max-width:65%; height:auto; display:block;" src="../assets/current-orders.png" />
    </el-header>
    <el-main>
      <el-table :data="outstandingOrders" style="width: 100%">
        <el-table-column type="expand">
          <template slot-scope="props">
            <el-table style="width:100%" :data="props.row.orderItems">
              <el-table-column label="Menu Item #" prop="menuItemId"></el-table-column>
              <el-table-column label="Name" prop="menuItemName"></el-table-column>
              <el-table-column label="Quantity" prop="quantity"></el-table-column>
            </el-table>
          </template>
        </el-table-column>
        <el-table-column label="Order #" prop="orderId"></el-table-column>
        <el-table-column label="Customer" prop="customerName"></el-table-column>
        <!-- <el-table-column label="Loyalty #" prop="loyaltyId"></el-table-column> -->
        <el-table-column label="Price ($)" prop="orderTotal"></el-table-column>
      </el-table>
    </el-main>
  </el-container>
</template>


<script>
export default {
  data() {
    return {
      connection: null,
      loading: true,
      orderSummary: {}
    };
  },
  beforeCreate() {
    this.$store.dispatch("fetchOutstandingOrders");
  },
  created() {
    // Listen to changes coming from SignalR events
    this.$orderHub.$on("new-order-received", this.newOrder);
    this.$orderHub.$on("order-completed", this.completedOrder);
  },
  beforeDestroy() {
    // Make sure to cleanup SignalR event handlers when removing the component
    this.$orderHub.$off("new-order-received", this.newOrder);
    this.$orderHub.$off("order-completed", this.completedOrder);
  },
  computed: {
    outstandingOrders() {
      return this.$store.getters.outstandingOrders;
    },
    ordersRetrieved() {
      return this.$store.getters.ordersRetrieved;
    },
    percentage() {
      return this.$store.getters.orderCompletionPercentage;
    },
    status() {
      if (this.percentage < 50) {
        return "exception";
      } else if (this.percentage >= 50 && this.percentage < 80) {
        return "warning";
      } else this.percentage >= 80 && this.props <= 100;
      {
        return "success";
      }
    }
  },
  methods: {
    refreshData() {
      this.$store.dispatch("fetchOutstandingOrders");
    },
    completedOrder(message) {
      this.orderSummary =  message;
      console.log("completed order: " + this.orderSummary.orderId);
      message.id = this.orderSummary.orderId;
      let index = this.findWithAttr(
        this.outstandingOrders,
        "orderId",
        this.orderSummary.orderId
      );
      if (index >= 0) {
        this.outstandingOrders.splice(index, 1);
      }

      this.$notify.info({
        title: "Order completed for customer " + this.orderSummary.customerName,
        order: "Order ID: " + this.orderSummary.orderId
      });

      this.$store.dispatch("orderItemCompleted");
    },
    findWithAttr(array, attr, value) {
      for (let i = 0; i < array.length; i += 1) {
        if (array[i][attr] === value) {
          return i;
        }
      }
      return -1;
    },
    newOrder(message) {
      this.orderSummary = message;
      console.log("new order: " + this.orderSummary.orderId);

      message.id = this.orderSummary.orderId;

      this.outstandingOrders.unshift(message);

      this.$notify.success({
        title: "Order received for customer " + this.orderSummary.customerName,
        message: "Order ID: " + this.orderSummary.orderId
      });
    }
  }
};
</script>

<style scoped>
.right {
  float: right;
  width: 110px;
}

.right .el-tooltip__popper {
  padding: 8px 10px;
}

.item {
  margin: 4px;
}

.bottom {
  clear: both;
  text-align: center;
}
</style>

