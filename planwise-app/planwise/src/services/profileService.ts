import api from "@/services/api"

export async function getRecentSearches() {
  const res = await api.get<string[]>("/api/ai/recent-searches")
  return res.data
}

export async function getSavedPlans() {
  const res = await api.get<string[]>("/api/ai/saved-plans")
  return res.data
}