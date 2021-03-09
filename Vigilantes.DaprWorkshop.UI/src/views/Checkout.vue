<template>
  <el-container>
    <el-header>
      <el-steps :active="active" style="margin:30px;">
        <el-step title="Enter Info" icon="el-icon-edit"></el-step>
        <el-step title="Review Order" icon="el-icon-coffee"></el-step>
        <el-step title="Submit" icon="el-icon-sell"></el-step>
      </el-steps>
    </el-header>
    <el-main>
      <el-form label-position="left" label-width="200px" v-if="active===1">
        <el-form-item label="Name">
          <el-input v-model="customerInfo.customerName" placeholder="Name"></el-input>
        </el-form-item>
        <el-form-item label="Loyalty Number">
          <el-input v-model="customerInfo.loyaltyId" placeholder="Loyalty #"></el-input>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" @click="saveCustomerState">Continue</el-button>
        </el-form-item>
      </el-form>
      <div v-if="active===2">
        <cart style="margin:30px auto;"></cart>
        <el-popconfirm @onConfirm="onConfirm"
          confirmButtonText="Yes"
          cancelButtonText="Cancel"
          icon="el-icon-info"
          iconColor="red"
          title="Are you sure to submit your order?">
          <el-button
            style="float:right!important;"
            slot="reference"           
          >Submit Order</el-button>
        </el-popconfirm>
      </div>
    </el-main>
  </el-container>
</template>

<script>
import Cart from "@/components/menu/Cart.vue";
import { mapState, mapGetters } from "vuex";

export default {
  data() {
    return {
      active: 1,
      fullscreenLoading: false
    };
  },
  computed: {
    ...mapState(["customerInfo"]),
    ...mapGetters(["customerInfo"]),
    checkoutStatus() {
      return this.$store.getters.checkoutStatus;
    }
  },
  components: {
    Cart
  },
  methods: {
    saveCustomerState() {
      this.$store.dispatch("setCustomerInfo", this.customerInfo);
      this.active++;
    },
   async onConfirm() {
        this.active++;
        
        const loading = this.$loading({
          lock: true,
          text: 'Processing order',
        });
      
        await this.$store.dispatch("checkout");

        if (this.checkoutStatus == "successful") {
          loading.close();
          this.$message({
            type: "success",
            message: "Thanks! Your order has been submitted successfully!",
            showClose: true,
            duration: 1000
          });
          
          this.$router.push("/");
        }
        else{
          loading.close();

          this.$message({
          type: "error",
          message: "There was an error submitting your order, please try again later!",
          showClose: true,
          duration: 2000
        });
        }
    }
  }
};
</script>

<style scoped>
.el-form {
  max-width: 500px;
  padding: 10px 20px;
  background: #f4f7f8;
  margin: 30px auto;
  padding: 20px;
  background: #f4f7f8;
  border-radius: 8px;
  font-family: Georgia, "Times New Roman", Times, serif;
}
</style>
