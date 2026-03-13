<template>
  <v-app-bar color="white" elevation="1">
    <v-app-bar-title class="brand-title">PlanWise AI</v-app-bar-title>

    <v-spacer></v-spacer>

    <template v-if="auth.isAuthenticated.value">
      <v-btn variant="text" class="nav-btn" prepend-icon="mdi-home-outline" to="/">Home</v-btn>
      <v-btn variant="text" class="nav-btn" prepend-icon="mdi-view-dashboard-outline" to="/dashboard">Dashboard</v-btn>
      <v-btn variant="text" class="nav-btn" prepend-icon="mdi-account-outline" to="/profile">Profile</v-btn>
      <v-btn v-if="auth.isAdmin.value" variant="text" class="nav-btn" prepend-icon="mdi-shield-crown-outline" to="/admin/dashboard">Admin</v-btn>
      <v-btn variant="text" class="nav-btn" prepend-icon="mdi-logout" @click="handleLogout">Logout</v-btn>
    </template>

    <template v-else>
      <v-btn variant="text" class="nav-btn" prepend-icon="mdi-login" to="/login">Login</v-btn>
      <v-btn color="primary" class="nav-btn" prepend-icon="mdi-account-plus-outline" to="/register">Sign Up</v-btn>
    </template>
  </v-app-bar>
</template>

<script setup lang="ts">
import { useRouter } from "vue-router"
import { useAuth } from "@/composables/useAuth"

const router = useRouter()
const auth = useAuth()

function handleLogout() {
  auth.logout()
  router.push("/login")
}
</script>

<style scoped>
.brand-title {
  font-size: 20px;
  font-weight: 700;
}

.nav-btn {
  text-transform: none;
  font-size: 14px;
  font-weight: 600;
  letter-spacing: 0;
}

@media (max-width: 900px) {
  .brand-title {
    font-size: 18px;
  }

  .nav-btn {
    min-width: 0;
    padding-inline: 8px;
    font-size: 12px;
  }
}

@media (max-width: 640px) {
  .brand-title {
    font-size: 16px;
  }

  .nav-btn :deep(.v-btn__prepend) {
    margin-inline-end: 0;
  }

  .nav-btn :deep(.v-btn__content) {
    font-size: 0;
  }
}
</style>