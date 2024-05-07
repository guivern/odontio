import { useDisplay } from 'vuetify';
import { computed } from 'vue';

export function isMobile() {
  const { name } = useDisplay();
  console.log(name.value);
  return computed(() => name.value === 'sm' || name.value === 'xs');
}
