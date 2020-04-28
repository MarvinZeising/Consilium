<template>
  <v-menu
    v-model="model"
    :close-on-content-click="false"
    offset-x
    offset-overflow
    :content-class="$vuetify.breakpoint.xsOnly ? 'menu-fullscreen' : ''"
  >
    <template v-slot:activator="{ on }">
      <v-btn icon v-on="on">
        <v-icon>info</v-icon>
      </v-btn>
    </template>

    <v-card
      min-width="350px"
      max-width="500px"
      :elevation="$vuetify.breakpoint.xsOnly ? 8 : undefined"
    >
      <v-toolbar dark flat short>
        <v-toolbar-title>Marvin Zeising</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn icon :title="$t('core.close')" @click="model = false">
          <v-icon>close</v-icon>
        </v-btn>
      </v-toolbar>

      <v-list>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-subtitle>Versammlung</v-list-item-subtitle>
            <v-list-item-title>Wuppertal-Nord</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-subtitle>Vorrechte</v-list-item-subtitle>
            <v-list-item-title>Dienstamtgehilfe / Pionier</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-subtitle>Telefon</v-list-item-subtitle>
            <v-list-item-title>+49 0177 1234567</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-subtitle>Sprachen</v-list-item-subtitle>
            <v-list-item-title>Deutsch, Englisch, Spanisch</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-subtitle>Besondere Informationen</v-list-item-subtitle>
            <v-list-item-title>Ich hab keinen Bock!</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-card>
  </v-menu>
</template>

<style lang="scss" scoped>
.menu-fullscreen {
  top: 0px !important;
  left: 0px !important;
  min-width: 100% !important;
  padding: 12px !important;
  box-shadow: none;
}
</style>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import UserModule from '../../store/users'
import PersonModule from '../../store/persons'

@Component
export default class PersonInfoMenu extends Vue {
  private userModule = getModule(UserModule, this.$store)
  private personModule = getModule(PersonModule, this.$store)

  @Prop(Number)
  private readonly personId!: number

  @Prop(Date)
  private readonly date?: Date

  private model: any
}
</script>
