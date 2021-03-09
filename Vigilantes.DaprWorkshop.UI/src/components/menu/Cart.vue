--<template>
  <el-container>
    <el-main>
      <el-table
        :summary-method="getSummaries"
        sum-text="Totals"
        show-summary
        style="width:100%"
        max-height="500"
        :data="cartItems"
      >
        <el-table-column fixed prop="menuItem.name" label="Name"></el-table-column>
        <el-table-column prop="quantity" label="Quantity">
          <template slot-scope="scope">
            <button
              @click="decreaseQuantity(scope.row.menuItem.menuItemId)"
              :disabled="scope.row.quantity === 1"
              class="btn btn-outline-danger btn-small"
            >-</button>
            <span style="margin-left: 10px; margin-right: 10px;">{{ scope.row.quantity }}</span>
            <button
              @click="increaseQuantity(scope.row.menuItem.menuItemId)"
              class="btn btn-outline-success btn-small"
            >+</button>
          </template>
        </el-table-column>
        <el-table-column prop label="Total Item Cost">
          <template slot-scope="scope">
            <span>{{ '$ ' + lineItemCost(scope.row) }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Operations">
          <template slot-scope="scope">
            <el-button size="mini" type="danger" @click="deleteCartItem(scope.$index)">Delete</el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-main>
  </el-container>
</template>

<script>
import { mapState, mapGetters } from "vuex";
export default {
  data() {
    return {
      quantity: 0
    };
  },
  computed: {
    ...mapState(["cartItems"]),
    ...mapGetters(["cartItems", "cartItemCount", "cartTotalAmount"])
  },
  methods: {
    lineItemCost(cartItem) {
      return cartItem.quantity * cartItem.menuItem.price;
    },
    increaseQuantity(id) {
      this.$store.dispatch("increaseQuantity", id);
    },
    decreaseQuantity(id) {
      this.$store.dispatch("decreaseQuantity", id);
    },
    deleteCartItem(index) {
      this.$store.dispatch("deleteCartItem", index);
    },
    getSummaries() {
      var summary = ["Total", "", "$ " + this.cartTotalAmount];
      return summary;
    }
  }
};
</script>