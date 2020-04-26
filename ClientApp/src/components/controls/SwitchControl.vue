<template>
  <div :class="'flex xs12 ' + getSm + getMd + getLg + 'pa-2'">
    <span v-if="description">{{ $t(description) }}</span>

    <v-switch
      v-model="model.value"
      :label="label ? $t(label) : false"
      color="primary"
      inset
      hide-details="auto"
      :disabled="disabled"
      @change="emitChange"
    />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop, Emit } from 'vue-property-decorator'
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

  @Prop(Boolean)
  private readonly disabled?: boolean

  @Prop(Number)
  private columns: 1 | 2 | 3 | 4 = 3

  private get getSm() {
    return this.columns >= 2 ? 'sm6 ' : ''
  }

  private get getMd() {
    return this.columns >= 3 ? 'md4 ' : ''
  }

  private get getLg() {
    return this.columns >= 4 ? 'lg3 ' : ''
  }

  @Emit('change')
  private emitChange() {
    return this.model?.value
  }
}
</script>
