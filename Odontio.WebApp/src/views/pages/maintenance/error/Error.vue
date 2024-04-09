<template>
  <error-card :code="errorCode" :message="errorMessage" />
</template>
<script setup>
import { useRouter } from "vue-router";
import ErrorCard from "@/components/shared/ErrorCard.vue";

const router = useRouter();
const errorMessage =
  router.currentRoute.value.query["message"] || "Unknown error";
const errorCode = router.currentRoute.value.query["code"] || "500";
</script>
