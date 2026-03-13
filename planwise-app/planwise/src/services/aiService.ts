import api from "@/services/api"

export interface GeneratedEventPlan {
  title: string
  description: string
  budgetLevel: string
  suggestedGroupSize: number
  locationType: string
  foodIdeas: string[]
  activities: string[]
  decorations: string[]
  score: number
}

interface GenerateIdeasRequest {
  prompt: string
}

async function generateEventIdeas(prompt: string): Promise<GeneratedEventPlan[]> {
  const response = await api.post<GeneratedEventPlan[]>("/api/ai/generate", {
    prompt
  } satisfies GenerateIdeasRequest)

  return response.data
}

export const aiService = {
  generateEventIdeas
}

export async function savePlan(title: string) {
  await api.post("/api/ai/save-plan", {
    title
  })
}
