<template>
  <el-container style="height: 100%">
    <el-header>
      <Header />
    </el-header>
    <el-main>
      <slot />
    </el-main>
    <el-footer>
      <Footer />
    </el-footer>
  </el-container>
</template>

<script lang="ts">
import Header from "../components/Header.vue";
import Footer from "../components/Footer.vue";
export default {
  components: {
    Header,
    Footer,
  },
};
</script>