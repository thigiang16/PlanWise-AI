<template>
  <v-app>

    <!-- NAVBAR -->
    <v-app-bar flat elevation="1">

      <v-container class="topbar d-flex align-center">

        <div class="logo">
          PlanWise <span class="ai-badge">AI</span>
        </div>

        <v-spacer></v-spacer>

        <v-btn variant="text" class="topbar-btn" to="/">
          Home
        </v-btn>

        <v-btn variant="text" class="topbar-btn" to="/dashboard">
          Dashboard
        </v-btn>

        <v-btn variant="text" class="topbar-btn" prepend-icon="mdi-account-outline" to="/profile">
          Profile
        </v-btn>

        <v-btn v-if="auth.isAdmin.value" variant="text" class="topbar-btn" to="/admin/dashboard">
          Admin
        </v-btn>

        <v-btn variant="text" class="topbar-btn" prepend-icon="mdi-logout" @click="handleLogout">
          Logout
        </v-btn>

      </v-container>

    </v-app-bar>

    <v-main>
      <v-container class="profile-page py-8">

        <div class="heading-wrap mb-6">
          <h1 class="text-h4">My Profile</h1>
          <p class="subtitle">Your account info and activity in one place.</p>
        </div>

        <!-- USER INFO -->
        <v-card class="pa-6 mb-6 section-card account-card" rounded="xl" elevation="3">
          <h2 class="text-h6 mb-4">Account Information</h2>

          <div class="info-row">
            <span class="label">Name</span>
            <strong>{{ auth.userName.value || "-" }}</strong>
          </div>

          <div class="info-row">
            <span class="label">Email</span>
            <strong>{{ auth.userEmail.value || "-" }}</strong>
          </div>
        </v-card>

        <!-- SAVED PLANS -->
        <v-card class="pa-6 mb-6 section-card saved-card" rounded="xl" elevation="3">
          <h2 class="text-h6 mb-4">Saved Plans</h2>

          <v-list v-if="savedPlans.length" class="list-surface">
            <v-list-item
              v-for="plan in savedPlans"
              :key="plan"
              prepend-icon="mdi-heart"
            >
              {{ plan }}
            </v-list-item>
          </v-list>

          <p v-else class="text-medium-emphasis">
            No saved plans yet.
          </p>
        </v-card>

        <!-- RECENT SEARCHES -->
        <v-card class="pa-6 section-card search-card" rounded="xl" elevation="3">
          <h2 class="text-h6 mb-4">Recent Searches</h2>

          <v-list v-if="recentSearches.length" class="list-surface">
            <v-list-item
              v-for="search in recentSearches"
              :key="search"
              prepend-icon="mdi-magnify"
            >
              {{ search }}
            </v-list-item>
          </v-list>

          <p v-else class="text-medium-emphasis">
            No recent searches.
          </p>
        </v-card>

      </v-container>
    </v-main>

  </v-app>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue"
import { useRouter } from "vue-router"
import { useAuth } from "@/composables/useAuth"
import { getRecentSearches, getSavedPlans } from "../services/profileService"

const router = useRouter()
const auth = useAuth()

const savedPlans = ref<string[]>([])
const recentSearches = ref<string[]>([])

function handleLogout() {
  auth.logout()
  router.push("/login")
}

onMounted(async () => {
  savedPlans.value = await getSavedPlans()
  recentSearches.value = await getRecentSearches()
})
</script>

<style scoped>
.profile-page {
  max-width: 900px;
  margin: auto;
}

.heading-wrap {
  border-left: 5px solid #14b8a6;
  padding-left: 12px;
}

.subtitle {
  color: #5f6b7a;
  margin-top: 6px;
}

.section-card {
  border: 1px solid rgba(20, 184, 166, 0.14);
}

.account-card {
  background: linear-gradient(180deg, #f0fdfa 0%, #ffffff 100%);
}

.saved-card {
  background: linear-gradient(180deg, #ecfeff 0%, #ffffff 100%);
}

.search-card {
  background: linear-gradient(180deg, #f8fafc 0%, #ffffff 100%);
}

.info-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 0;
  border-bottom: 1px solid #e2e8f0;
}

.info-row:last-child {
  border-bottom: 0;
}

.label {
  color: #64748b;
}

.list-surface {
  background: rgba(255, 255, 255, 0.65);
  border-radius: 12px;
}

.logo {
  font-weight: 700;
}

.ai-badge {
  background: teal;
  color: white;
  padding: 2px 8px;
  border-radius: 10px;
  font-size: 12px;
  margin-left: 4px;
}

@media (max-width: 640px) {
  .topbar {
    flex-wrap: wrap;
    row-gap: 8px;
    padding-block: 10px;
  }

  .topbar-btn {
    min-width: 0;
    padding-inline: 8px;
  }

  .topbar-btn :deep(.v-btn__prepend) {
    margin-inline-end: 0;
  }

  .topbar-btn :deep(.v-btn__content) {
    font-size: 0;
  }

  .profile-page {
    padding-top: 12px;
  }

  .info-row {
    flex-direction: column;
    align-items: flex-start;
    gap: 6px;
  }
}
</style>