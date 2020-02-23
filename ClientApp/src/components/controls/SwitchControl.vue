<template>
  <v-flex xs12 sm6 md4 class="pa-2">
    <span v-if="description">{{ $t(description) }}</span>

    <v-switch
      v-model="model.value"
      :label="label ? $t(label) : false"
      color="primary"
      inset
      hide-details
      @change="toggleTeamCaptain"
    />
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'

@Component
export default class SwitchControl extends Vue {
  @Prop(Object)
  private readonly model?: { value: string }

  @Prop(String)
  private readonly label?: string

  @Prop(String)
  private readonly description?: string
}
</script>
