export type GetUsersDto = {
  id: number;
  username: string;
  role: string;
  workspaceId: number;
  isActive: boolean;
  workspaceName: string;
  roleName: string;
  isDoctor: boolean;
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  photoUrl: string | null;
};

export type UpsertUserDto = {
  username: string | null;
  password: string | null;
  confirmPassword: string | null;
  roleId: number | null;
  workspaceId: number | null;
  isActive: boolean;
  isDoctor: boolean;
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  photoUrl: string | null;
};

export type ResetPasswordDto = {
  password: string;
  confirmPassword: string;
};