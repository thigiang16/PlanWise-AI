<template>
  <v-app>
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
      <v-container class="dashboard py-8">
        <v-card class="prompt-area pa-6" rounded="xl" elevation="3">
          <div class="heading-wrap">
            <h1>What kind of event are you planning?</h1>

            <p class="subtitle">
              Describe your event and let our AI find the perfect plan for you.
            </p>
          </div>

          <v-textarea
            v-model="prompt"
            placeholder="Small indoor party with 5 people..."
            rows="3"
            variant="outlined"
            class="prompt-input mt-4"
          />

          <v-btn
            color="teal"
            size="large"
            class="generate-btn"
            :loading="isGenerating"
            :disabled="isGenerating"
            @click="generateIdeas"
          >
            ✨ Generate Ideas
          </v-btn>

          <v-alert
            v-if="errorMessage"
            type="error"
            variant="tonal"
            class="mt-4"
          >
            {{ errorMessage }}
          </v-alert>
        </v-card>

        <div v-if="results.length" class="results">
          <div class="results-header">
            <h2>Recommended Event Plans</h2>

            <span class="result-count">
              {{ results.length }} results found
            </span>
          </div>

          <v-card
            v-for="plan in results"
            :key="plan.title"
            class="result-card"
            rounded="xl"
            elevation="3"
          >
            <div class="card-header">
              <h3>{{ plan.title }}</h3>

              <v-chip
                v-if="plan === results[0]"
                color="green"
                size="small"
              >
                Top Match
              </v-chip>

              <v-chip
                color="teal"
                size="small"
                variant="tonal"
              >
                {{ plan.score }}% Match
              </v-chip>
            </div>

            <p class="description">
              {{ plan.description }}
            </p>

            <div class="tags">
              <v-chip size="small">Budget: {{ plan.budgetLevel }}</v-chip>
              <v-chip size="small">Group Size: {{ plan.suggestedGroupSize }}</v-chip>
              <v-chip size="small">{{ plan.locationType }}</v-chip>
            </div>

            <div class="actions">
              <v-btn
                color="teal"
                size="small"
                @click="viewDetails(plan)"
              >
                View Details
              </v-btn>

              <v-btn
                variant="outlined"
                size="small"
                @click="toggleFavorite(plan)"
              >
                {{ plan.saved ? '❤ Saved' : '♡ Save' }}
              </v-btn>
            </div>
          </v-card>
        </div>

        <v-card
          v-else-if="!isGenerating"
          class="empty-state mt-8 pa-8 text-center"
          rounded="xl"
          variant="tonal"
        >
          <v-icon size="34" color="teal" class="mb-2">mdi-lightbulb-on-outline</v-icon>
          <h3 class="text-h6">No ideas yet</h3>
          <p class="text-medium-emphasis">Enter an event prompt and generate your first recommendations.</p>
        </v-card>
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { useRouter } from "vue-router"
import { useAuth } from "@/composables/useAuth"
import { aiService, savePlan, type GeneratedEventPlan } from "@/services/aiService"

type GeneratedEventPlanWithSaved = GeneratedEventPlan & {
  saved?: boolean
}

const prompt = ref("")
const router = useRouter()
const auth = useAuth()

const results = ref<GeneratedEventPlanWithSaved[]>([])
const isGenerating = ref(false)
const errorMessage = ref("")

async function generateIdeas() {
  if (!prompt.value.trim()) {
    errorMessage.value = "Please describe your event first."
    results.value = []
    return
  }

  isGenerating.value = true
  errorMessage.value = ""

  try {
    const plans = await aiService.generateEventIdeas(prompt.value)
    results.value = plans.map(plan => ({ ...plan, saved: false }))
  } catch {
    results.value = []
    errorMessage.value = "Unable to generate ideas right now. Please try again."
  } finally {
    isGenerating.value = false
  }
}

function handleLogout() {
  auth.logout()
  router.push("/login")
}

async function toggleFavorite(plan: GeneratedEventPlanWithSaved) {
  if (!plan.saved) {
    await savePlan(plan.title)
    plan.saved = true
  }
}

function viewDetails(plan: GeneratedEventPlanWithSaved) {
  sessionStorage.setItem("selectedPlan", JSON.stringify(plan))
  router.push({ name: "plan-details" })
}
</script>

<style scoped>
.dashboard {
  max-width: 900px;
  margin: auto;
}

.prompt-area {
  text-align: center;
  background: linear-gradient(180deg, #f0fdfa 0%, #ffffff 100%);
  border: 1px solid rgba(20, 184, 166, 0.2);
}

.heading-wrap {
  border-left: 5px solid #14b8a6;
  padding-left: 12px;
  text-align: left;
}

.prompt-input {
  text-align: left;
}

.subtitle {
  color: #64748b;
  margin-bottom: 10px;
}

.generate-btn {
  margin-top: 10px;
  width: 300px;
  max-width: 100%;
}

.results {
  margin-top: 50px;
}

.results-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  gap: 12px;
}

.result-card {
  padding: 20px;
  margin-bottom: 18px;
  border: 1px solid rgba(20, 184, 166, 0.16);
  background: linear-gradient(180deg, #f8fafc 0%, #ffffff 100%);
}

.card-header {
  display: flex;
  align-items: center;
  gap: 10px;
  flex-wrap: wrap;
}

.description {
  margin: 10px 0;
  color: #475569;
}

.tags {
  display: flex;
  gap: 8px;
  margin-bottom: 10px;
  flex-wrap: wrap;
}

.actions {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.empty-state {
  border: 1px dashed rgba(20, 184, 166, 0.35);
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

@media (max-width: 900px) {
  .results-header {
    flex-direction: column;
    align-items: flex-start;
  }
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

  .prompt-area {
    padding: 18px !important;
  }

  .prompt-area h1 {
    font-size: 32px;
    line-height: 1.15;
  }

  .generate-btn,
  .actions .v-btn {
    width: 100%;
  }
}
</style>
