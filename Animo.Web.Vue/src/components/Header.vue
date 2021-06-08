<template>
  <el-menu mode="horizontal" @select="handleSelect">
    <el-row type="flex">
      <el-col :span="12">
        <el-row type="flex" align="middle" justify="start">
          <router-link to="/"> Animo </router-link>
          <router-link to="/About">
            <el-menu-item index="1">Browse</el-menu-item>
          </router-link>
        </el-row>
      </el-col>
      <el-col :span="12">
        <el-row type="flex" align="middle" justify="end">
          <el-space :size="24">
            <router-link to="/">
              <el-menu-item index="2">Watchlist </el-menu-item>
            </router-link>
            <router-link to="/About">
              <el-menu-item index="3">Calendar</el-menu-item>
            </router-link>
            <router-link to="/">
              <el-menu-item index="4">Ratings</el-menu-item>
            </router-link>
            <router-link to="/Login">
              <el-link href="/Login">Login</el-link>
            </router-link>
          </el-space>
        </el-row>
      </el-col>
    </el-row>
  </el-menu>
</template>

<script>
export default {
  data() {
    return {
      activeIndex: "1",
    };
  },
  methods: {
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    },
  },
};
</script>
