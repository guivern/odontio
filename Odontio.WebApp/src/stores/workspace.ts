import { defineStore } from 'pinia';
import type { WorkspaceDto } from '@/types/workspace';

export const useWorkspace = defineStore({
  id: 'workspace',
  state: () => ({
    workspace: localStorage.getItem('workspace') ? JSON.parse(localStorage.getItem('workspace') as string) : null
  }),
  actions: {
    setWorkspace(workspace: WorkspaceDto) {
      this.workspace = workspace;
      localStorage.setItem('workspace', JSON.stringify(this.workspace));
    },
    clearWorkspace() {
      this.workspace = null;
      localStorage.removeItem('workspace');
    }
  }
});
