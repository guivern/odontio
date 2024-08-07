export const useFormatter = () => {
  const toCurrency = (value: number): string => {
    return new Intl.NumberFormat('es-PY' /*{ style: 'currency', currency: 'PYG' }*/).format(value);
  };

  const toDateOnly = (value: string): string => {
    return new Date(value).toISOString().split('T')[0];
  };

  const toNumber = (value: string): number => {
    return Number(value);
  }

  return {
    toCurrency,
    toDateOnly,
    toNumber
  };
};
