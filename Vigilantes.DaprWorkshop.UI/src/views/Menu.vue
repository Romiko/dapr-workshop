<template>
  <el-container>
      <el-header height="100px" style="margin-top:50px">
      <img
        style="max-width:65%; height:auto; display:block; margin-left:auto; margin-right:auto"
        src="../assets/menu-header.png"
      />
    </el-header>
    <el-main>
      <el-row>
        <menu-item v-for="menuItem in menuItems" :key="menuItem.menuItemId" :menuItem="menuItem"></menu-item>
      </el-row>
    </el-main>
    <cart></cart>
    <el-footer>
      <el-button
        style="float:right!important;"
        @click="checkout"
        :disabled="orderQuantity <= 0"
      >Checkout</el-button>
    </el-footer>
  </el-container>
</template>

<script>
import MenuItem from "@/components/menu/MenuItem.vue";
import Cart from "@/components/menu/Cart.vue";

export default {
  data(){
    return{
    };
  },
  components: {
    MenuItem,
    Cart
  },
  async created() { 
    await this.$store.dispatch("fetchMenuItems");
  },
  computed: {
    menuItems() {
      return this.$store.getters.menuItems;
    },
    orderQuantity() {
      return this.$store.getters.cartItemCount;
    }
  },
  methods: {
    checkout() {
      this.$router.push("checkout");
    }
  }
};
</script>

<style scoped>
.el-header {
  background-color: transparent;
  color: #333;
  line-height: 60px;
}
</style>