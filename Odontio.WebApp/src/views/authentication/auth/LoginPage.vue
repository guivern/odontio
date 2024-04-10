<script setup lang="ts">
import Logo from '@/layouts/full/logo/LogoDark.vue';
import AuthLogin from '../authForms/AuthLogin.vue';
</script>

<template>
  <v-row class="h-100vh" no-gutters>
    <!---Left Part-->
    <v-col cols="12" class="d-flex align-center bg-lightprimary">
      <v-container>
        <div class="pa-7 pa-sm-12">
          <v-row justify="center">
            <v-col cols="12" lg="10" xl="6" md="7">
              <v-card elevation="0" class="loginBox">
                <v-card variant="outlined">
                  <v-card-text class="pa-9">
                    <!---Left Part Logo -->
                    <v-row>
                      <v-col cols="12" class="text-center">
                        <!-- <Logo /> -->
                        <!-- <div d-flex align-center>
                          <Logo />
                          <span class="text-h3" style="color: #283593">Odontio</span>
                        </div> -->
                        <div class="d-flex justify-center align-center">
                          <Logo width="44" height="44" />
                          <span class="text-h3" style="color: #283593">Odontio</span>
                        </div>
                        <h2 class="text-secondary text-h2 mt-3">Hola, Bienvenido! 👋</h2>
                        <h4 class="text-disabled text-h4 mt-3">Ingresa tus credenciales para continuar</h4>
                      </v-col>
                    </v-row>
                    <!---Left Part Logo -->

                    <!---Left Part Form-->
                    <AuthLogin />
                    <!---Left Part Form-->
                  </v-card-text>
                </v-card>
              </v-card>
            </v-col>
          </v-row>
        </div>
      </v-container>
    </v-col>
    <!---Left Part-->
  </v-row>
</template>
<style lang="scss">
.loginBox {
  max-width: 475px;
  margin: 0 auto;
}
</style>
