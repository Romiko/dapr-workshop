<template>
  <el-container>
    <el-header height="100px" style="margin-top:50px">
      <div class="right">
        <el-tooltip class="item" effect="dark" content="Fetch new orders" placement="right">
          <el-button @click="refreshData">Refresh</el-button>
        </el-tooltip>
      </div>
      <img
        style="max-width:65%; height:auto; display:block;"
        src="../assets/current-orders.png"
      />
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
        <!-- <el-table-column label="Loyalty Id" prop="loyaltyId"></el-table-column> -->
        <el-table-column label="Price ($)" prop="orderTotal"></el-table-column>
      </el-table>
    </el-main>
  </el-container>
</template>


<script>
export default {
  data() {
    return {
    };
  },
  created() {
    this.$store.dispatch("fetchOutstandingOrders");
  },
  computed: {
    outstandingOrders() {
      return this.$store.getters.outstandingOrders;
    }
  },
  methods: {
    refreshData(){
      this.$store.dispatch("fetchOutstandingOrders");
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

