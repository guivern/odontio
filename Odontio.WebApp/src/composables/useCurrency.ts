// src/composables/useCurrencyFormatter.ts
export const useCurrency = () => {
  const formatCurrency = (value: number): string => {
    return new Intl.NumberFormat('es-PY' /*{ style: 'currency', currency: 'PYG' }*/).format(value);
  };

  return {
    formatCurrency
  };
};
