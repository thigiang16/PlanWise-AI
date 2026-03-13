<template>
  <div class="landing">

    <!-- NAVBAR -->
    <v-app-bar flat color="transparent" class="navbar">
      <v-container class="d-flex align-center justify-space-between">

        <div class="logo">
          PlanWise <span class="ai-badge">AI</span>
        </div>

        <div class="nav-links">
          <a href="#">Home</a>
          <a href="#">How It Works</a>
          <a href="#">About</a>
        </div>

        <div class="auth-buttons">
          <v-btn variant="text" to="/login">Login</v-btn>

          <v-btn color="teal" rounded to="/register">
            Sign Up
          </v-btn>
        </div>

      </v-container>
    </v-app-bar>

    <!-- HERO -->
    <section class="hero">

      <div class="overlay"></div>

      <v-container class="hero-content">

        <h1>Plan Smarter. Celebrate Better.</h1>

        <p>
          Describe your event and let us create personalized ideas for you.
        </p>

        <!-- SEARCH -->
        <div class="search-box">

          <v-text-field
            v-model="prompt"
            placeholder="E.g. 'Fun summer hangout with friends'"
            variant="solo"
            hide-details
          />

          <v-btn
            color="teal"
            size="large"
            @click="generateIdeas"
          >
            Get Ideas
          </v-btn>

        </div>

        <!-- FAKE PREVIEW -->
        <transition name="fade-slide" appear>
          <div v-if="showPreview" class="preview-area">

            <div class="preview-backdrop" @click="showPreview = false"></div>

            <div class="preview-shell">

              <v-skeleton-loader
                type="card, article, actions"
                class="preview-card"
              />

              <div class="preview-overlay">

                <v-btn
                  icon="mdi-close"
                  variant="text"
                  class="close-btn"
                  @click="showPreview = false"
                />

                <h2>Your plan is ready 🎉</h2>

                <p>
                  Create a free account to see your full event plan.
                </p>

                <div class="preview-buttons">

                  <v-btn color="teal" to="/register">
                    Sign Up Free
                  </v-btn>

                  <v-btn variant="outlined" to="/login">
                    Login
                  </v-btn>

                </div>

              </div>
            </div>

          </div>
        </transition>

        <!-- FEATURES -->
        <div class="features">

          <div class="feature">
            <v-icon size="36">mdi-lightbulb-outline</v-icon>
            <h3>Creative Ideas</h3>
            <p>Generate fun and unique event suggestions.</p>
          </div>

          <div class="feature">
            <v-icon size="36">mdi-calendar-check-outline</v-icon>
            <h3>Easy Planning</h3>
            <p>Get actionable plans tailored to your group.</p>
          </div>

          <div class="feature">
            <v-icon size="36">mdi-heart-outline</v-icon>
            <h3>Save & Expand</h3>
            <p>Save favorites and expand plans with AI.</p>
          </div>

        </div>

        <!-- CTA -->
        <div class="cta">

          <v-btn color="primary" size="large" to="/login">
            Login
          </v-btn>

          <v-btn color="teal" size="large" to="/register">
            Sign Up Free
          </v-btn>

        </div>

      </v-container>

    </section>

  </div>
</template>

<script setup lang="ts">
import { ref } from "vue"

const prompt = ref("")
const showPreview = ref(false)

const generateIdeas = () => {

  if (!prompt.value) return

  showPreview.value = true

}
</script>

<style scoped>

/* GLOBAL */

.landing {
  font-family: "Inter", sans-serif;
  scroll-behavior: smooth;
}

/* NAVBAR */

.navbar {
  position: absolute;
  width: 100%;
  z-index: 10;

  background: rgba(255,255,255,0.08);
  backdrop-filter: blur(14px);

  border-bottom: 1px solid rgba(255,255,255,0.15);
}

.logo {
  font-size: 22px;
  font-weight: 700;
  color: white;
  transition: transform .25s ease;
}

.logo:hover {
  transform: translateY(-1px);
}

.ai-badge {
  background: linear-gradient(135deg,#14b8a6,#0d9488);
  padding: 3px 9px;
  border-radius: 14px;
  font-size: 12px;
  margin-left: 4px;
  transition: box-shadow .25s ease;
}

.logo:hover .ai-badge {
  box-shadow: 0 0 0 3px rgba(20, 184, 166, .2);
}

.nav-links a {
  margin: 0 14px;
  color: white;
  text-decoration: none;
  opacity: .9;
  transition: opacity .25s ease, transform .25s ease;
  display: inline-block;
}

.nav-links a:hover {
  opacity: 1;
  transform: translateY(-1px);
}

/* HERO */

.hero {
  position: relative;
  height: 100vh;

  background-image: url('/event-bg.jpg');
  background-size: cover;
  background-position: center;

  display: flex;
  align-items: center;
  justify-content: center;
}

.overlay {
  position: absolute;
  inset: 0;
  background: rgba(0,0,0,.6);

  padding-top: 72px;
}
.hero-content {
  position: relative;
  text-align: center;
  color: white;
  max-width: 900px;
}

.hero-content > * {
  animation: rise-in .6s ease both;
}

.hero-content > :nth-child(1) { animation-delay: .05s; }
.hero-content > :nth-child(2) { animation-delay: .12s; }
.hero-content > :nth-child(3) { animation-delay: .18s; }
.hero-content > :nth-child(4) { animation-delay: .24s; }

.hero-content h1 {
  font-size: 56px;
  margin-bottom: 16px;
}

.hero-content p {
  font-size: 20px;
  margin-bottom: 36px;
}

/* SEARCH */

.search-box {
  display: flex;
  gap: 12px;
  max-width: 620px;
  margin: auto;
  margin-bottom: 40px;
  transition: transform .25s ease, box-shadow .25s ease;
  border-radius: 12px;
}

.search-box:focus-within {
  transform: translateY(-1px);
  box-shadow: 0 8px 24px rgba(0,0,0,.25);
}

/* PREVIEW */

.preview-area {
  position: fixed;
  inset: 0;
  z-index: 30;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 24px;
}

.preview-backdrop {
  position: absolute;
  inset: 0;
  background: rgba(2, 6, 23, .7);
  backdrop-filter: blur(8px);
}

.preview-shell {
  position: relative;
  width: min(700px, 100%);
}

.preview-card {
  filter: blur(5px);
  opacity: .6;
  border-radius: 16px;
  overflow: hidden;
}

.preview-overlay {
  position: absolute;
  inset: 0;

  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;

  background: rgba(0,0,0,.6);
  backdrop-filter: blur(10px);

  border-radius: 16px;
  padding: 28px;
}

.close-btn {
  position: absolute;
  top: 8px;
  right: 8px;
  color: white;
}

.preview-overlay h2 {
  margin-bottom: 12px;
  flex-wrap: wrap;
  justify-content: center;
}

.preview-buttons {
  display: flex;
  gap: 14px;
  margin-top: 16px;
}

/* FEATURES */

.features {
  display: flex;
  justify-content: center;
  gap: 60px;
  margin-top: 60px;
}

.feature {
  max-width: 220px;
  padding: 10px;
  border-radius: 14px;
  background: rgba(255, 255, 255, .06);
  border: 1px solid rgba(255, 255, 255, .12);
  transition: transform .25s ease, background-color .25s ease;
}

.feature:hover {
  transform: translateY(-4px);
  background: rgba(255, 255, 255, .1);
}

/* CTA */

.cta {
  margin-top: 40px;
  display: flex;
  justify-content: center;
  gap: 16px;
}

.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: opacity .35s ease, transform .35s ease;
}

.fade-slide-enter-from,
.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(12px);
}

@keyframes rise-in {
  from {
    opacity: 0;
    transform: translateY(12px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* MOBILE */

@media (max-width:900px){

.features{
flex-direction:column;
align-items:center;
}

.search-box{
flex-direction:column;
}

}

</style>