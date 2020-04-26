<template>
  <div :class="'flex xs12 ' + getSm + getMd + getLg + 'pa-2'">
    <span v-if="showDescription">
      {{ $t(description) }}
      <i v-if="!required">({{ $t('core.optional') }})</i>
    </span>
    <v-textarea
      v-model="model.value"
      :label="label ? $t(label) + $t('core.multilineSuffix') : false"
      :rules="rules"
      :counter="getCounter"
      rows="1"
      auto-grow
      filled
      required
    />
  </div>
</template>

<script lang="ts">
import { Vue, Component, Watch, Prop } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'

@Component
export default class TextareaControl extends Vue {
  @Prop(Object)
  private readonly model?: { value: string }

  @Prop(String)
  private readonly label?: string

  @Prop(String)
  private readonly description?: string

  @Prop(Number)
  private readonly minLength?: number

  @Prop(Number)
  private readonly maxLength?: number

  @Prop(Boolean)
  private showDescription?: boolean

  @Prop(Boolean)
  private required = false

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

  private get getCounter() {
    if (this.model && this.maxLength && this.maxLength > 0) {
      return this.model.value.length >= this.maxLength * 0.8 ? this.maxLength : false
    }
    return false
  }

  private rules: any[] = [
    (v: string) => !this.required || !!v || i18n.t('core.fieldRequired'),
    (v: string) =>
      this.minLength === undefined ||
      v.length >= this.minLength ||
      i18n.t('core.fieldMin', { count: this.minLength }),
    (v: string) =>
      this.maxLength === undefined ||
      v.length <= this.maxLength ||
      i18n.t('core.fieldMax', { count: this.maxLength }),
  ]
}
</script>
